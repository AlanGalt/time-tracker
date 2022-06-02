import axios from 'axios';

export default class TaskService {
  API_URL = import.meta.env.VITE_API_URL;

  async addTask(task) {
    const response = await axios.post(`${this.API_URL}/issues`, task);
    return response.data;
  }

  async deleteTask(id) {
    const response = await axios.delete(`${this.API_URL}/issues/${id}`);
    return response.data;
  }

  async editTask(id, newTask) {
    const response = await axios.put(`${this.API_URL}/issues/${id}`, newTask);
    return response.data;
  }

  async getActiveTasks() {
    const response = await axios.get(`${this.API_URL}/issues?activeonly=true`);
    return response.data
  }
}