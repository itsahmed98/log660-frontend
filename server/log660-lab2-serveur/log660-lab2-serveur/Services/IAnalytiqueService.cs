namespace log660_lab2_serveur.Services
{
    public interface IAnalytiqueService
    {
        Task<int> GetNombreLocations(string? groupeAge, string? province, string? jourSemaine, string? moisAnnee);
    }
}
