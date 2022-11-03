import { storage } from "../firebase";
import { ref, uploadBytesResumable } from "firebase/storage";

const Uploader = {
  upload(file, name) {
    if (file == null) return;
    const storageRef = ref(storage, `/images/${name}`);
    const uploadTask = uploadBytesResumable(storageRef, file);
    uploadTask.on(
      "state_changed",
      (error) => {
        return error;
      },
      () => {
        storageRef
          .getDownloadURL()
          .then((url) => {
            console.log(url);
            return url;
          })
          .catch((error) => {
            return error;
          });
      }
    );
  }
};

export default Uploader;
