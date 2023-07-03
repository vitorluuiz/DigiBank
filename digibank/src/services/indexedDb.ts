// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable no-console */
const dbName = 'digibank';
const dbVersion = 1;

interface IndexedImage {
  id: number;
  img: string;
}

export const openDB = () => {
  const request = indexedDB.open(dbName, dbVersion);

  request.onupgradeneeded = (event: any) => {
    const db = event.target.result;
    const objectStore = db.createObjectStore('images', { keyPath: 'id', autoIncrement: true });
    console.log(objectStore);
  };

  request.onsuccess = (event: any) => {
    const db = event.target.result;
    db.close();
  };

  request.onerror = (event1: any) => {
    console.error(`Error opening database: ${event1.target.error}`);
  };
};

export const removeImage = (imageId: number) => {
  const request = indexedDB.open(dbName);
  const db = request.result;
  const transaction = db.transaction(['images'], 'readwrite');
  const objectStore = transaction.objectStore('images');

  const deleteRequest = objectStore.delete(imageId);

  deleteRequest.onsuccess = () => {
    console.log('Imagem removida com sucesso!');
  };

  deleteRequest.onerror = (event: any) => {
    console.error(`Erro ao remover imagem: ${event.target.error}`);
  };
};

export const postImage = (image: IndexedImage) => {
  const request = indexedDB.open(dbName);

  request.onsuccess = (event: any) => {
    const db = event.target.result;
    const transaction = db.transaction(['images'], 'readwrite');
    const objectStore = transaction.objectStore('images');

    const addRequest = objectStore.add(image);

    addRequest.onsuccess = (eventSla: any) => {
      console.log(`Post cadastrado com sucesso! ID: ${eventSla.target.result}`);
    };

    addRequest.onerror = (eventError: any) => {
      console.error(`Erro ao cadastrar post: ${eventError.target.error}`);
    };
  };

  request.onerror = (event: any) => {
    console.error(`Erro ao abrir o banco de dados: ${event.target.error}`);
  };
};

export const getAllEntities = (objectStoreName: string) => {
  const request = indexedDB.open(dbName);

  request.onsuccess = (event: any) => {
    const db = event.target.result;
    const transaction = db.transaction(objectStoreName, 'readonly');
    const objectStore = transaction.objectStore(objectStoreName);
    const getAllRequest = objectStore.getAll();

    getAllRequest.onsuccess = (eventSucess: any) => {
      const entities = eventSucess.target.result;
      console.log(entities);
      // Faça algo com a lista de entidades
    };

    getAllRequest.onerror = (eventError: any) => {
      console.log('Erro ao obter entidades:', eventError.target.error);
    };
  };

  request.onerror = (event: any) => {
    console.log('Erro ao abrir o banco de dados:', event.target.error);
  };
};
