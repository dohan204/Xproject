namespace TestX.api.Dtos.Permission
{
    public class Tree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FunctionSto> Functions { get; set; }
    }
    public class FunctionSto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permissionst> Permissions { get; set; }
    }
    public class Permissionst
    {
        public bool canCreate { get; set;  }
        public bool canDelete { get; set; }
        public bool canModify { get; set; }
        public bool canView { get; set; }
    }
}
