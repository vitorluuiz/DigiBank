// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable react/require-default-props */
import React, { CSSProperties } from 'react';

interface Style {
  box?: CSSProperties;
  firstName?: CSSProperties;
  lastName?: CSSProperties;
  title?: CSSProperties;
}

interface LinearRatingProps {
  value: number; // Valor do índice (0 a 10)
  firstName?: string;
  lastName?: string;
  title?: string;
  adornment?: string;
  styles?: Style;
}

export const LinearRating: React.FC<LinearRatingProps> = ({
  value,
  firstName,
  lastName,
  title,
  adornment,
  styles,
}) => {
  // Limitar o valor entre 0 e 10
  const ratingValue = Math.max(0, Math.min(10, value));

  // Calcular a largura da barra em relação ao valor
  const barWidth = `${ratingValue * 10}%`;

  function barColor() {
    let haxColor: string;

    switch (ratingValue.toFixed(0).toString()) {
      case '3':
        haxColor = '#FF1103';
        break;
      case '4':
        haxColor = '#FB2700';
        break;
      case '5':
        haxColor = '#FF8D00';
        break;
      case '6':
        haxColor = '#FEC002';
        break;
      case '7':
        haxColor = '#ACDD29';
        break;
      case '8':
        haxColor = '#00CB15';
        break;
      case '9':
        haxColor = '#04C82C';
        break;
      case '10':
        haxColor = '#90086B';
        break;
      default:
        haxColor = '000';
        break;
    }

    return haxColor;
  }

  return (
    <div id="linear-rating-bar" style={styles?.box}>
      <div className="names">
        <span style={styles?.firstName} className="titulo-linear-bar">
          {firstName}
        </span>
        <span style={styles?.lastName} className="titulo-linear-bar">
          {lastName}
        </span>
      </div>
      <div className="support-linear-bar">
        <div className="background-bar">
          <div
            className="rating-bar"
            style={{
              width: barWidth,
              backgroundColor: barColor(),
            }}
          />
        </div>
        <span className="rating-linear-bar">
          {ratingValue.toPrecision(2)}
          {adornment}
        </span>
      </div>
      <span style={styles?.title}>{title}</span>
    </div>
  );
};

export const LinearProgression: React.FC<LinearRatingProps> = ({
  value,
  firstName,
  lastName,
  title,
  adornment,
  styles,
}) => {
  // Limitar o valor entre 0 e 10
  const ratingValue = Math.max(0, Math.min(100, value));

  // Calcular a largura da barra em relação ao valor
  const barWidth = `${ratingValue}%`;

  function barColor() {
    let haxColor: string;

    switch (parseInt(ratingValue.toFixed(0), 10)) {
      default:
        haxColor = '#000';
        break;
    }

    return haxColor;
  }

  return (
    <div id="linear-rating-bar" style={styles?.box}>
      <div className="names">
        <span style={styles?.firstName} className="titulo-linear-bar">
          {firstName}
        </span>
        <span style={styles?.lastName} className="titulo-linear-bar">
          {lastName}
        </span>
      </div>
      <div className="support-linear-bar">
        <div className="background-bar">
          <div
            className="rating-bar"
            style={{
              width: barWidth,
              backgroundColor: barColor(),
            }}
          />
        </div>
        <span className="rating-linear-bar">
          {ratingValue.toFixed(2)}
          {adornment}
        </span>
      </div>
      <span style={styles?.title}>{title}</span>
    </div>
  );
};
