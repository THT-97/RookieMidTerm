// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getStorage } from "firebase/storage";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDIe6yOKW0oecUpcEBJM6et4FE0S13lGds",
  authDomain: "ecommerce-images-c6324.firebaseapp.com",
  projectId: "ecommerce-images-c6324",
  storageBucket: "ecommerce-images-c6324.appspot.com",
  messagingSenderId: "15729910515",
  appId: "1:15729910515:web:4ace5229a4f03ac68f4d86"
};

// Initialize Firebase
export const app = initializeApp(firebaseConfig);
export const storage = getStorage(app);
