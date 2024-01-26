using TaskHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskHub.Models.WorkSpaceViewModels
{
    public class UserIndexData
{
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<WorkSpace> WorkSpaces { get; set; }
        public IEnumerable<WorkSpaceMember> WorkSpaceMembers { get; set; }
    }
}