using _3dprint_inventory_api.Models;

namespace _3dprint_inventory_api.Schema;

public interface IBaseSchema<TSchemaModel, TDbModel>
where TDbModel : BaseEntity
where TSchemaModel : class, IBaseSchema<TSchemaModel, TDbModel>
{
    public TDbModel ToDbModel();
    public abstract static TSchemaModel FromDbModel(TDbModel model);
}