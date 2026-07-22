import { Routes, Route } from 'react-router-dom'; // 1. Import Routes and Route
import './styles/global.scss'
import { InventoryLayout } from './layout/inventory/inventoryLayout';
import { Dashboard } from './pages/inventory/dashboard';
import { Inventory } from './pages/inventory/inventory';
import { Products } from './pages/inventory/products';
import { Recieve } from './pages/inventory/recieve';
import { Reports } from './pages/inventory/reports';
import { Sales } from './pages/inventory/sales';
import { Settings } from './pages/inventory/settings';
import { Suppliers } from './pages/inventory/suppliers';
import { Login } from './pages/auth/login';
import { Register } from './pages/auth/register';
import { AuthLayout } from './layout/auth/authLayout';

export const App = () => {
  return (
    <Routes>
      <Route path="/reisa" element={<AuthLayout />}>
        <Route index element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="login" element={<Login />} />
      </Route>


      <Route path="/reisa" element={<InventoryLayout />}>
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="inventory" element={<Inventory />} />
        <Route path="products" element={<Products />} />
        <Route path="recieve" element={<Recieve />} />
        <Route path="contact" element={<Reports />} />
        <Route path="contact" element={<Sales />} />
        <Route path="contact" element={<Settings />} />
        <Route path="contact" element={<Suppliers />} />
      </Route>
    </Routes>
  )
};