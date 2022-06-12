

export default class TimeEntryModel {
  constructor(timeEntry, date, taskId, hours, description) {
    if (timeEntry) {
      this.id = timeEntry.timeEntryId;
      this.taskId = timeEntry.issueId;
      this.date = timeEntry.timeEntryDate;
      this.hours = timeEntry.timeEntryHours;
      this.description = timeEntry.timeEntryDescription;
    } else {
      this.date = date;
      this.issueId = taskId;
      this.hours = hours;
      this.description = description;
    }
    
  }
}