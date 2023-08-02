// import React, { useEffect, useState } from 'react';
// import Footer from '../../../components/Footer';
// import Header from '../../../components/Header';
// import api from '../../../services/api';
// import { parseJwt } from '../../../services/auth';
// import { WishlishedPost } from '../../DigiStore/Post';
// import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';

// export default function FavortosInvest() {
//   const [investList, setInvestList] = useState<[]>([]);

//   const GetWishlistFromServer = (idsInvestimento: number[]) => {
//     api.post(`InvestimentoOptions/Favoritos`, idsInvestimento).then((response) => {
//       if (response.status === 200) {
//         setInvestList(response.data);
//       }
//     });
//   };

//   const GetWishlistFromLocal = () => {
//     if (localStorage.getItem('wishlist')) {
//       const localData: WishlishedPost[] = JSON.parse(localStorage.getItem('wishlist') ?? '[]');
//       const idInvestimentos: number[] = [];
//       localData.forEach((item) => {
//         if (item.idUsuario === parseJwt().role) {
//           idInvestimentos.push(item.idUsuario); // alterar aqui
//         }
//       });
//       GetWishlistFromServer(idInvestimentos);
//     } else {
//       localStorage.setItem('wishlist', '[]');
//     }
//   };

//   useEffect(() => {
//     GetWishlistFromLocal();
//     // eslint-disable-next-line react-hooks/exhaustive-deps
//   }, []);

//   return (
//     <div>
//       <Header type="digiStore" />
//       <main id="catalogo" className="container">
//         <div className="support-recomendados-post">
//           <div className="recomendados-list-support">
//             <h2>Classificado por itens desejados</h2>
//             <div className="recomendados-list extended-list">
//               {/* Postagem */}
//               {investList.map((investimento) => (
//                 <RecommendedInvestiment
//                   type="slim"
//                   key={investimento.idInvestimento}
//                   investimento={investimento}
//                 />
//               ))}
//             </div>
//           </div>
//         </div>
//       </main>
//       <Footer />
//     </div>
//   );
// }
