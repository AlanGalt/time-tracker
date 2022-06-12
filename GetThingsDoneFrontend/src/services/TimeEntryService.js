import axios from 'axios'

export default class TimeEntryService {
  API_URL = import.meta.env.VITE_API_URL;

  async getAllEntries(date, monthOnly) {
    let query = '';

    if (date) query = `?date=${date}&monthOnly=${monthOnly}`;
    
    const response = await axios.get(`${this.API_URL}/time-entries${query}`);
    return response.data;
  }

  async addTimeEntry(entry) {
    const response = await axios.post(`${this.API_URL}/time-entries`, entry);
    return response.data;
  }

  async deleteTimeEntry(id) {
    const response = await axios.delete(`${this.API_URL}/time-entries/${id}`);
    return response.data;
  }

  async editTimeEntry(id, newEntry) {
    const response = await axios.put(`${this.API_URL}/time-entries/${id}`, newEntry);
    return response.data;
  }
};