using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskHub.Models.WorkSpaceViewModels
{
    public class WorkSpaceIndexData
{
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<WorkSpace> WorkSpaces { get; set; }
        public IEnumerable<Board> Boards { get; set; }
        public IEnumerable<List> Lists { get; set; }
        public IEnumerable<TaskItem> TaskItems { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<WorkSpaceMember> WorkSpaceMembers { get; set; }
    }
}