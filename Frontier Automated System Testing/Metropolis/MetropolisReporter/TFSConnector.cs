using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace CoffeeBeanReporter
{
    class TFSConnector
    {
        
        private WorkItemStore _store;
        private TfsTeamProjectCollection _tfs;
        private ITestManagementTeamProject _teamProject;

        /**
         * This will take the TFS Project
         */
        public void setTFSProject()
        {
            TeamProjectPicker tpp = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, false);
            
            //Following actions will be executed only if a team project is selected in the the opened dialog.
            if (tpp.SelectedTeamProjectCollection != null)
            {
                this._tfs = tpp.SelectedTeamProjectCollection;

                ITestManagementService test_service = (ITestManagementService)_tfs.GetService(typeof(ITestManagementService));

                //tmService = (ITestManagementService)_tfs.GetService(typeof(ITestManagementService));

                _store = (WorkItemStore)_tfs.GetService(typeof(WorkItemStore));

                this._teamProject = test_service.GetTeamProject(tpp.SelectedProjects[0].Name);
                
                //Call to method "Get_TestPlans" to get the test plans in the selected team project
                //Get_TestPlans(_teamProject);

            }
            else
            {
                System.Environment.Exit(1);
            }
        }

    }
}
