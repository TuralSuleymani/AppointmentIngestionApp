import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AppointmentsPage } from './pages/AppointmentsPage';
import { AppointmentDetailsPage } from './pages/AppointmentDetailsPage';

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<AppointmentsPage />} />
          <Route path="/appointments/:id" element={<AppointmentDetailsPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;