using System.Collections.Generic;

namespace EzyWizard;

public class EzyWizard
{
	private List<IWizardForm> m_wizardFormsList;

	public EzyWizard()
	{
		m_wizardFormsList = new List<IWizardForm>();
	}

	public void AddForm(IWizardForm wizardForm)
	{
		m_wizardFormsList.Add(wizardForm);
	}

	public WizardResults Start(ref IWizardData data, ref IWizardData updatedData)
	{
		updatedData.Copy(data);
		WizardResults wizardResults = new WizardResults();
		wizardResults.ExitState = RunWizard(updatedData, m_wizardFormsList);
		return wizardResults;
	}

	private ExitStateEnum RunWizard(IWizardData data, List<IWizardForm> wizardFormList)
	{
		int indexOfLast = m_wizardFormsList.Count - 1;
		WizardFormResults wizardFormResults = new WizardFormResults();
		bool flag = false;
		int num = 0;
		while (!flag)
		{
			wizardFormResults = DisplayForm(wizardFormList[num], num, indexOfLast, data);
			switch (wizardFormResults.ButtonPessed)
			{
			case ButtonPressedEnum.Next:
				num++;
				break;
			case ButtonPressedEnum.Back:
				num--;
				break;
			case ButtonPressedEnum.Finish:
				flag = true;
				break;
			case ButtonPressedEnum.Cancel:
				flag = true;
				break;
			default:
				return ExitStateEnum.Canceled;
			}
		}
		if (wizardFormResults.ButtonPessed == ButtonPressedEnum.Finish)
		{
			return ExitStateEnum.Finished;
		}
		return ExitStateEnum.Canceled;
	}

	private WizardFormResults DisplayForm(IWizardForm wizardForm, int positionInList, int indexOfLast, IWizardData data)
	{
		if (positionInList == indexOfLast)
		{
			return wizardForm.ShowDialog(FormPositionEnum.Last, ref data);
		}
		if (positionInList == 0)
		{
			return wizardForm.ShowDialog(FormPositionEnum.First, ref data);
		}
		return wizardForm.ShowDialog(FormPositionEnum.Middle, ref data);
	}
}
