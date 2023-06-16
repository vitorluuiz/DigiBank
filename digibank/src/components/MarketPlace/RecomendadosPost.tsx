// import RecommendedBlock from './RecommendedPost';
import { PostProps } from '../../@types/Post';
import Carousel from './Carousel';

export default function RecomendadosPost({ postprops }: { postprops: PostProps | undefined }) {
  // const [deProprietarioList, setDeProprietario] = useState<PostProps[]>([]);
  // const [topRatingList, setTopRatingList] = useState<PostProps[]>([]);

  return (
    <div className="support-recomendados-post">
      {/* Do mesmo anunciante */}
      <div className="recomendados-list-support">
        <h2>Do mesmo Anunciante</h2>
        <div className="recomendados-list">
          {/* Postagem */}
          {/* {deProprietarioList.map((post) => (
            <RecommendedBlock key={post.idPost} post={post} />
          ))} */}
          <Carousel type="anunciante" postprops={postprops} />
        </div>
      </div>

      {/* Recomendados */}
      <div className="recomendados-list-support">
        <h2>Produtos recomendados</h2>
        <div className="recomendados-list">
          {/* Postagem */}
          {/* Postagem */}
          {/* {topRatingList.map((post) => (
            <RecommendedBlock key={post.idPost} post={post} />
          ))} */}
          <Carousel type="slim" />
        </div>
      </div>
    </div>
  );
}
