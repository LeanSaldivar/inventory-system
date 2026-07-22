import { Routes, Route } from 'react-router-dom'; // 1. Import Routes and Route
import './styles/global.scss'
import { InventoryLayout } from './layout/inventory/inventoryLayout';
import { Dashboard } from './pages/dashboard';
import { Inventory } from './pages/inventory';
import { Products } from './pages/products';
import { Recieve } from './pages/recieve';
import { Reports } from './pages/reports';
import { Sales } from './pages/sales';
import { Settings } from './pages/settings';
import { Suppliers } from './pages/suppliers';
import { Login } from './pages/inventory/login';
import { Register } from './pages/inventory/register';
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