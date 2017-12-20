namespace IzgodnoKupi.Services.Contracts
{
    public interface IBagService
    {
        decimal TotalAmount(string id);

        int Count(string id);
    }
}
