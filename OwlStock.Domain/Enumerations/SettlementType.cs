using System.ComponentModel;

namespace OwlStock.Domain.Enumerations
{
    public enum SettlementType
    {
        [Description("Селo")]
        Village = 1,

        [Description("Град")]
        City = 2,

    }
}
