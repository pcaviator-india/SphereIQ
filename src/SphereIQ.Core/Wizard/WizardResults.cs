namespace EzyWizard;

public class WizardResults
{
	private ExitStateEnum m_exitState;

	public ExitStateEnum ExitState
	{
		get
		{
			return m_exitState;
		}
		set
		{
			m_exitState = value;
		}
	}
}
