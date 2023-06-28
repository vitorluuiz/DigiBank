// import RecommendedBlock from './RecommendedPost';
import { PostProps } from '../../@types/Post';
import CarouselPosts from './Carousel/CarouselPosts';

export default function RecomendadosPost({ postprops }: { postprops: PostProps | undefined }) {
  return (
    <div className="support-recomendados-post">
      {/* Do mesmo anunciante */}
      <div className="recomendados-list-support">
        <h2>Do mesmo Anunciante</h2>
        <div className="recomendados-list">
          {postprops !== undefined ? (
            <CarouselPosts type="anunciante" idOwner={postprops.idUsuario} />
          ) : null}
        </div>
      </div>

      <div className="recomendados-list-support">
        <h2>Produtos recomendados</h2>
        <div className="recomendados-list">
          <CarouselPosts type="slim" />
        </div>
      </div>
    </div>
  );
}
