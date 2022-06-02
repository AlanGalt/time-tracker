import axios from 'axios'

export default class TimeEntryService {
  API_URL = import.meta.env.VITE_API_URL;

  async getAllEntries() {
    const response = await axios.get(`${this.API_URL}/time-entries`);
    return response.data;
  } 

  // async getTasksForProject(id) {
  //   const response = await axios.get(`${this.API_URL}/projects/${id}/issues`);
  //   return response.data;
  // }

  async addTimeEntry(entry) {
    const response = await axios.post(`${this.API_URL}/time-entries`, entry);
    return response.data;
  }

  // async deleteProject(id) {
  //   const response = await axios.delete(`${this.API_URL}/projects/${id}`);
  //   return response.data;
  // }

  // async editProject(id, newProject) {
  //   const response = await axios.put(`${this.API_URL}/projects/${id}`, newProject);
  //   return response.data;
  // }
};