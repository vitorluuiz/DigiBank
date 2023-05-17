import React from 'react';
import { Link } from 'react-router-dom';

function SideBar() {
  return (
    <aside>
      <Link to="/diginvest">Diginvest</Link>
      <Link to="/digistore  ">Digistore</Link>
    </aside>
  );
}

export default SideBar;
