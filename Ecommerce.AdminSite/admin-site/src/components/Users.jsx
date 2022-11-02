import axios from "axios";
import { useState, useEffect, React } from "react";

const Users = () => {
  const [users, setUsers] = useState(null);
  useEffect(() => {
    axios
      .get("https://localhost:7171/api/User/GetAll")
      .then((response) => setUsers(response.data))
      .catch((error) => setUsers(error.data));
  }, []);

  let table = [];
  if (users != null) {
    // add data objects to a table variable
    table = users.map((user) => (
      <tr key={user.id}>
        <td>{user.id}</td>
        <td>{user.userName}</td>
        <td>{user.email}</td>
      </tr>
    ));
  }

  return (
    <div className="col-8 p-3">
      <h1>User list</h1>
      <hr />
      <table id="table" className="table table-striped table-hover">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Email</th>
          </tr>
        </thead>
        <tbody>
          {/* render table var inside tbody */}
          {table}
        </tbody>
      </table>
    </div>
  );
};

export default Users;
