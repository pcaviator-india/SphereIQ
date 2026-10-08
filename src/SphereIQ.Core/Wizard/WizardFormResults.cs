namespace EzyWizard;

public class WizardFormResults
{
	private ButtonPressedEnum m_buttonPressed;

	public ButtonPressedEnum ButtonPessed
	{
		get
		{
			return m_buttonPressed;
		}
		set
		{
			m_buttonPressed = value;
		}
	}

	public WizardFormResults()
	{
		ButtonPessed = ButtonPressedEnum.Cancel;
	}
}
