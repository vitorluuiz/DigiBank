// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable no-nested-ternary */
import React, { useEffect, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';
import Rating from '@mui/material/Rating';
import CircularProgress from '@mui/material/CircularProgress/CircularProgress';

import { CommentProps } from '../../../@types/Comment';
import ModalComentario from '../ModalComentarPost';
import { PostProps } from '../../../@types/Post';
import Histograma from '../../RatingStats';
import { RatingHistograma } from '../../../@types/RatingHistogram';
import { parseJwt } from '../../../services/auth';
import CommentPost from './Comments';
import api from '../../../services/api';

export default function AvaliacoesPost({
  postProps,
  reqUpdate,
}: {
  postProps: PostProps;
  reqUpdate: () => void;
}) {
  const [comments, setComments] = useState<CommentProps[]>([]);
  const [canComment, setCanComment] = useState<boolean>(false);
  const [commentsHistograma, setCommentsHistograma] = useState<RatingHistograma[]>([]);
  const [commentPage, setCommentPage] = useState<number>(1);
  const [hasMore, setHasMore] = useState<boolean>(false);
  const [isLoading, setLoading] = useState<boolean>(false);

  function GetComments() {
    setHasMore(false);
    setLoading(true);

    const itensPerPage = 4;

    api(
      `Avaliacoes/AvaliacoesPost/${postProps?.idPost}/${
        parseJwt().role === 'undefined' ? 0 : parseJwt().role
      }/${commentPage}/${itensPerPage}`,
    ).then((response) => {
      if (response.status === 200) {
        const { avaliacoesList, ratingHistograma, canPostComment } = response.data;

        if (commentPage === 1) {
          setComments(avaliacoesList);
        } else {
          setComments([...comments, ...avaliacoesList]);
        }

        setCommentsHistograma(ratingHistograma);
        setCanComment(canPostComment);
        setHasMore(true);
        setLoading(false);

        if (avaliacoesList.length < itensPerPage) {
          setHasMore(false);
        }
      }
    });
  }

  const resetPages = () => {
    if (commentPage === 1) {
      GetComments();
    } else {
      setCommentPage(1);
    }
  };

  const plusPage = () => setCommentPage(commentPage + 1);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => resetPages(), [postProps]);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => GetComments(), [commentPage]);

  return (
    <div id="mainAvaliacoes" className="support-avaliacoes-post">
      <div className="avaliacao">
        <div className="avaliacoes-stats">
          <div id="avaliacoes-post">
            <div>
              <span id="nota">{postProps?.avaliacao}</span>
              <Rating value={postProps?.avaliacao ?? 0} precision={0.1} size="large" readOnly />
            </div>
            <span>{postProps?.qntAvaliacoes} avaliações</span>
          </div>
          <Histograma histograma={commentsHistograma} />
        </div>
        {postProps?.idUsuario.toString() !== parseJwt().role && canComment ? (
          <ModalComentario
            onUpdate={() => {
              reqUpdate();
            }}
            postProps={postProps}
            type="comentar"
            comment={{
              comentario: '',
              dataPostagem: new Date(),
              idAvaliacao: 0,
              idUsuario: parseJwt().role,
              isReplied: false,
              nota: 1,
              publicador: '',
              replies: 0,
            }}
          />
        ) : null}
      </div>
      <InfiniteScroll
        dataLength={comments.length}
        next={plusPage}
        hasMore={hasMore}
        loader={<CircularProgress color="inherit" />}
        className="boxScrollInfinito comments-list"
      >
        {comments.map((comment) => (
          <CommentPost
            key={comment.idAvaliacao}
            onUpdate={() => {
              reqUpdate();
            }}
            comment={comment}
            postProps={postProps}
          />
        ))}
        {isLoading ? <CircularProgress color="inherit" /> : undefined}
      </InfiniteScroll>
    </div>
  );
}
