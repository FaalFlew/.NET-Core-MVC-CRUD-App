import React, { useState } from "react";

interface AddUserFormProps {
  record: {
    username: string;
    dateJoined: string;
    email: string;
    password: string;
  };
  onAdd: (record: {
    username: string;
    dateJoined: string;
    email: string;
    password: string;
  }) => void;
  validateEmail: (email: string) => boolean;
}

const AddUserForm: React.FC<AddUserFormProps> = ({
  record,
  onAdd,
  validateEmail,
}) => {
  const [newRecord, setNewRecord] = useState(record);

  const handleSubmit = () => {
    const { username, dateJoined, email, password } = newRecord;

    if (!username || !dateJoined || !email || !password) {
      alert("All fields are required.");
      return;
    }

    if (!validateEmail(email)) {
      alert("Enter a valid email address.");
      return;
    }

    onAdd(newRecord);
    setNewRecord({ username: "", dateJoined: "", email: "", password: "" }); // Reset form after successful submission
  };

  return (
    <div className="card mt-4">
      <div className="card-body">
        <h5 className="card-title">Add New User</h5>
        <input
          type="text"
          className="form-control"
          value={newRecord.username}
          onChange={(e) =>
            setNewRecord({ ...newRecord, username: e.target.value })
          }
          placeholder="Username"
        />
        <input
          type="date"
          className="form-control mt-2"
          value={newRecord.dateJoined}
          max="4444-12-31"
          onChange={(e) =>
            setNewRecord({ ...newRecord, dateJoined: e.target.value })
          }
        />
        <input
          type="email"
          className="form-control mt-2"
          value={newRecord.email}
          onChange={(e) =>
            setNewRecord({ ...newRecord, email: e.target.value })
          }
          placeholder="Email"
        />
        <input
          type="password"
          className="form-control mt-2"
          value={newRecord.password}
          onChange={(e) =>
            setNewRecord({ ...newRecord, password: e.target.value })
          }
          placeholder="Password"
        />
        <button className="btn btn-success mt-3" onClick={handleSubmit}>
          Add
        </button>
      </div>
    </div>
  );
};

export default AddUserForm;
