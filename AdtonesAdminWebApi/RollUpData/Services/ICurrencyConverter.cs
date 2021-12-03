namespace AdtonesAdminWebApi.RollUpData.Services
{
    public interface ICurrencyConverter
    {
        int DisplayCurrencyId { get; }
        decimal ConvertToDisplay(decimal value, int fromCurrencyId);
    }
}