namespace BlogProject.Models
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<String>();
        }

        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

    }
}
