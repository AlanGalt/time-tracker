

export default class ProjectModel {
  constructor(project, name, code) {
    if (project) {
      this.id = project.projectId;
      this.name = project.projectName;
      this.code = project.projectCode;
      this.active = project.projectActive;
    } else {
      this.name = name;
      this.code = code;
      this.active = true;
    }
  }
}