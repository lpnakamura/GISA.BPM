using GISA.BPM.Domain.Entities;
using System;

namespace GISA.BPM.Domain.Extensions
{
    public static class EntityBaseExtension
    {
        public static EntityBase ToUpdate(this EntityBase entity, string updatedBy)
        {
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = updatedBy;
            return entity;
        }

        public static EntityBase ToInsert(this EntityBase entity, string updatedBy)
        {
            entity.CreatedOn = DateTime.Now;
            entity.CreatedBy = updatedBy;
            entity.UpdatedOn = null;
            entity.UpdatedBy = null;
            return entity;
        }
    }
}
