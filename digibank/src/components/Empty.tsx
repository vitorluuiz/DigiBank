import React, { FC } from 'react';

import Logo from '../assets/img/passaroLogo.png';

interface EmptyProps {
  type: string;
}

const Empty: FC<EmptyProps> = ({ type }) => (
  <div>
    {type === 'metas' && (
      <div className="empty-center container">
        <img alt="Logo da Digibank" src={Logo} />
        <h2>Você não possui metas cadastradas.</h2>
        <span>Quando uma meta for cadastrada ela aparecera aqui.</span>
      </div>
    )}
    {type === 'pegarEmprestimo' && (
      <div className="empty-start container">
        <img alt="Logo da Digibank" src={Logo} />
        <h2>Você não possui permissão para adquirir um emprestimo.</h2>
      </div>
    )}
    {type === 'pagarEmprestimo' && (
      <div className="empty-start container">
        <img alt="Logo da Digibank" src={Logo} />
        <h2>Você não possui nenhum emprestimo a pagar.</h2>
      </div>
    )}
    {/* {type !== 'metas' && type !== 'pegarEmprestimo' && type !== 'pagarEmpretimo' <h2>salve</h2>} */}
  </div>
);

export default Empty;
