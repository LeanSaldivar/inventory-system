import { Routes, Route } from 'react-router-dom'; // 1. Import Routes and Route
import './styles/global.scss'
import { InventoryLayout } from './layout/inventory/inventoryLayout';
import { Dashboard } from './pages/inventory/dashboard';
import { Products } from './pages/inventory/products';
import { Sales } from './pages/inventory/sales';
import { Login } from './pages/auth/login';
import { Register } from './pages/auth/register';
import { AuthLayout } from './layout/auth/authLayout';
import { Ai } from './layout/ai/ai';
import { Reccomend } from './pages/ai/reccomend';
import { Prompt } from './pages/ai/prompt';

export const App = () => {
  return (
    <Routes>
      <Route path="/auth" element={<AuthLayout />}>
        <Route index element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="login" element={<Login />} />
      </Route>

      <Route path="/reisa" element={<InventoryLayout />}>
        <Route index element={<Dashboard />} />
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="products" element={<Products />} />
        <Route path="sales" element={<Sales />} />
      </Route>

      <Route path="/reisa/ai" element ={<Ai/>}>
        <Route index element={<Reccomend />} />
        <Route path="reccomend" element={<Reccomend />} />
        <Route path="prompt" element={<Prompt />} />
      </Route>
    </Routes>
  )
};