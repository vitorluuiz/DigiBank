export default function verificaPercentualBrancoImagem(imagemSrc: string) {
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
          const totalPixels = imageData.length / 4;
          let whitePixels = 0;

          for (let i = 0; i < imageData.length; i += 4) {
            const red = imageData[i];
            const green = imageData[i + 1];
            const blue = imageData[i + 2];

            if (red === 255 && green === 255 && blue === 255) {
              // eslint-disable-next-line no-plusplus
              whitePixels++;
            }
          }

          const percentualBranco = (whitePixels / totalPixels) * 100;
          const ehMajoritariamenteBranco = percentualBranco >= 50; // Defina o limite percentual aqui
          resolve(ehMajoritariamenteBranco);
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
