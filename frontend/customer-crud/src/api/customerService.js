import axios from 'axios';

const API_URL = 'https://localhost:7211/api/customer';

export const getCustomers = () => axios.get(API_URL).then(res => res.data);
export const createCustomer = (customer) => axios.post(API_URL, customer).then(res => res.data);
export const updateCustomer = (customer) => axios.put(API_URL, customer).then(res => res.data);
export const deleteCustomer = (id) => axios.delete(`${API_URL}/${id}`).then(res => res.data);
axios.interceptors.response.use(
    (response) => response,
    (error) => {
      alert(`Error: ${error.response?.data?.message || error.message}`);
      return Promise.reject(error);
    }
  );
  