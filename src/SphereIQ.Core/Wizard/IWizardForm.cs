namespace EzyWizard;

public interface IWizardForm
{
	WizardFormResults ShowDialog(FormPositionEnum position, ref IWizardData data);
}
