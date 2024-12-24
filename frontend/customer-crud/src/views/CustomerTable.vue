<template>
    <div>
      <h1>GESTIÓN DE CLIENTES</h1>
      <div class="controls">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Buscar por nombre"
          class="search-bar"
        />
        <button class="btn-add" @click="addNewRow">Agregar</button>
      </div>
  
      <div class="table-container">
        <table border="1">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Apellido</th>
              <th>Email</th>
              <th>Teléfono</th>
              <th>Dirección</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(customer, index) in filteredCustomers"
              :key="customer.id || index"
            >
              <td>
                <input
                  class="inputnew"
                  v-model="customer.firstName"
                  type="text"
                  placeholder="Nombre"
                  :disabled="!customer.isEditing"
                />
              </td>
              <td>
                <input
                  class="inputnew"
                  v-model="customer.lastName"
                  type="text"
                  placeholder="Apellido"
                  :disabled="!customer.isEditing"
                />
              </td>
              <td>
                <input
                  class="inputnew"
                  v-model="customer.email"
                  type="email"
                  placeholder="Email"
                  :disabled="!customer.isEditing"
                />
              </td>
              <td>
                <input
                  class="inputnew"
                  v-model="customer.phone"
                  type="text"
                  placeholder="Teléfono"
                  :disabled="!customer.isEditing"
                />
              </td>
              <td>
                <input
                  class="inputnew"
                  v-model="customer.address"
                  type="text"
                  placeholder="Dirección"
                  :disabled="!customer.isEditing"
                />
              </td>
              <td>
                <button
                  class="btn-edit"
                  v-if="!customer.isEditing"
                  @click="editCustomer(customer)"
                >
                  Editar
                </button>
                <button
                  class="btn-save"
                  v-if="customer.isEditing"
                  @click="saveCustomer(customer)"
                >
                  Guardar
                </button>
                <button
                  class="btn-delete"
                  v-if="customer.id"
                  @click="confirmDelete(customer)"
                >
                  Eliminar
                </button>
                <button
                  class="btn-cancel"
                  v-if="!customer.id"
                  @click="removeUnsavedRow(index)"
                >
                  Cancelar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <div class="card-container">
        <div
          class="card"
          v-for="(customer, index) in filteredCustomers"
          :key="customer.id || index"
        >
          <div>
            <label><strong>Nombre:</strong></label>
            <input
              v-if="customer.isEditing"
              v-model="customer.firstName"
              type="text"
              placeholder="Nombre"
              class="inputnew"
            />
            <span v-else>{{ customer.firstName }}</span>
          </div>
  
          <div>
            <label><strong>Apellido:</strong></label>
            <input
              v-if="customer.isEditing"
              v-model="customer.lastName"
              type="text"
              placeholder="Apellido"
              class="inputnew"
            />
            <span v-else>{{ customer.lastName }}</span>
          </div>
  
          <div>
            <label><strong>Email:</strong></label>
            <input
              v-if="customer.isEditing"
              v-model="customer.email"
              type="email"
              placeholder="Email"
              class="inputnew"
            />
            <span v-else>{{ customer.email }}</span>
          </div>
  
          <div>
            <label><strong>Teléfono:</strong></label>
            <input
              v-if="customer.isEditing"
              v-model="customer.phone"
              type="text"
              placeholder="Teléfono"
              class="inputnew"
            />
            <span v-else>{{ customer.phone }}</span>
          </div>
  
          <div>
            <label><strong>Dirección:</strong></label>
            <input
              v-if="customer.isEditing"
              v-model="customer.address"
              type="text"
              placeholder="Dirección"
              class="inputnew"
            />
            <span v-else>{{ customer.address }}</span>
          </div>
  
          <div class="card-actions">
            <button
              class="btn-edit"
              v-if="!customer.isEditing"
              @click="editCustomer(customer)"
            >
              Editar
            </button>
            <button
              class="btn-save"
              v-if="customer.isEditing"
              @click="saveCustomer(customer)"
            >
              Guardar
            </button>
            <button
              class="btn-delete"
              v-if="customer.id"
              @click="confirmDelete(customer)"
            >
              Eliminar
            </button>
            <button
              class="btn-cancel"
              v-if="!customer.id"
              @click="removeUnsavedRow(index)"
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
  
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <p>¿Estás seguro de que deseas eliminar a {{ customerToDelete?.firstName }}?</p>
          <button class="btn-delete" @click="deleteCustomer(customerToDelete.id)">Eliminar</button>
          <button class="btn-cancel" @click="closeModal">Cancelar</button>
        </div>
      </div>
    </div>
  </template>
  
  
<script setup>
import { ref, computed } from 'vue'; 

import {
  getCustomers,
  createCustomer,
  updateCustomer,
  deleteCustomer as deleteCustomerAPI,
} from '../api/customerService';

const customers = ref([]);
const searchQuery = ref('');
const showModal = ref(false);
const customerToDelete = ref(null);

const fetchCustomers = async () => {
  customers.value = await getCustomers();
};

const addNewRow = () => {
  customers.value.push({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    isEditing: true,
  });
};

const editCustomer = (customer) => {
  customer.isEditing = true;
};

const saveCustomer = async (customer) => {
  if (customer.id) {
    await updateCustomer(customer); // Actualizar cliente
  } else {
    const newCustomer = await createCustomer(customer); // Crear cliente nuevo
    Object.assign(customer, newCustomer); // Actualizar datos locales con el ID del servidor
  }
  customer.isEditing = false;
  fetchCustomers(); // Refrescar la lista
};

const confirmDelete = (customer) => {
  customerToDelete.value = customer;
  showModal.value = true;
};

const deleteCustomer = async (id) => {
  await deleteCustomerAPI(id);
  closeModal();
  fetchCustomers();
};

const removeUnsavedRow = (index) => {
  customers.value.splice(index, 1);
};

const closeModal = () => {
  showModal.value = false;
  customerToDelete.value = null;
};

const filteredCustomers = computed(() =>
  customers.value.filter((customer) =>
    `${customer.firstName} ${customer.lastName}`
      .toLowerCase()
      .includes(searchQuery.value.toLowerCase())
  )
);

fetchCustomers();
</script>
