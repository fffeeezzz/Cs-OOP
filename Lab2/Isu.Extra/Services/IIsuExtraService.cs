using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Models;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
    public Ognp AddOgnp(string name, char faculty);
    public StreamGroup AddStreamGroup(string groupName);
    public OgnpFlow AddOgnpFlowToOgnp(string name, char faculty, Ognp ognp);

    public StreamStudent AddStudentToStreamGroup(StreamGroup streamGroup, string name);
    public StreamStudent AddStudentToOgnpFlow(StreamStudent streamStudent, OgnpFlow ognpFlow);
    public void DropStudentOutFromOgnpFlow(StreamStudent streamStudent, OgnpFlow ognpFlow);

    public IEnumerable<StreamGroup> GetStreamGroupsByCourseNumber(CourseNumber courseNumber);
    public IEnumerable<StreamStudent> GetStreamStudentsFromOgnpFlow(OgnpFlow ognpFlow);
    public IEnumerable<StreamStudent> GetStreamStudentsWithoutOgnpFromStreamGroup(StreamGroup group);
}