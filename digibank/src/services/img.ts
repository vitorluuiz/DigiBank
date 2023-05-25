export default function verificaTransparenciaImagem(imagemSrc: string) {
  return new Promise((resolve, reject) => {
    const img = new Image();
    img.crossOrigin = 'anonymous';
    img.src = imagemSrc;

    img.onload = function load() {
      const canvas = document.createElement('canvas');
      canvas.width = img.width;
      canvas.height = img.height;
      const context = canvas.getContext('2d');

      if (context) {
        context.drawImage(img, 0, 0);

        try {
          const imageData = context.getImageData(0, 0, canvas.width, canvas.height).data;
          let transparentPixels = 0;
          const totalPixels = imageData.length / 4; // Cada pixel é composto por 4 bytes (RGBA)

          for (let i = 3; i < imageData.length; i += 4) {
            if (imageData[i] < 255) {
              // eslint-disable-next-line no-plusplus
              transparentPixels++;
            }
          }

          const transparenciaPercentual = (transparentPixels / totalPixels) * 100;
          const ehMajoritariamenteTransparente = transparenciaPercentual >= 50; // Defina o limite percentual aqui

          resolve(ehMajoritariamenteTransparente);
        } catch (error) {
          reject(error); // Ocorreu um erro ao obter os dados de pixel do canvas
        }
      }
    };

    img.onerror = function () {
      reject(new Error('Erro ao carregar a imagem.'));
    };
  });
}
