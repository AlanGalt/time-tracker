
export default class TaskModel {
  constructor(issue, projectId, taskName) {
    if (issue) {
      this.id = issue.issueId;
      this.name = issue.issueName;
      this.active = issue.issueActive;
      this.projectId = issue.projectId;
    } else {
      this.name = taskName;
      this.active = true;
      this.projectId = projectId;
    }
  }
}