namespace WixWPFWizardBA.Common
{
    public interface IInstallationTypeProvider<T>
    {
        T InstallationType { get; }
    }
}