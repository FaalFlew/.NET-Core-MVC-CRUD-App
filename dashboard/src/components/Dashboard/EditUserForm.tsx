import { useState } from "react";

interface EditUserFormProps {
    record: any;
    onSave: (record: any) => void;
    onCancel: () => void;
    validateEmail: (email: string) => boolean;
  }
  
  const EditUserForm: React.FC<EditUserFormProps> = ({
    record,
    onSave,
    onCancel,
    validateEmail,
  }) => {
    const [updatedRecord, setUpdatedRecord] = useState(record);
  
    const handleSave = () => {
      if (!validateEmail(updatedRecord.email)) {
        alert("Enter a valid email address");
        return;
      }
      onSave(updatedRecord);
    };
  
    return (
      <div className="col-md-4 mb-3">
        <div className="card">
          <div className="card-body">
            <input
              type="text"
              className="form-control"
              value={updatedRecord.username}
              onChange={(e) =>
                setUpdatedRecord({ ...updatedRecord, username: e.target.value })
              }
            />
            <input
              type="date"
              className="form-control mt-2"
              value={updatedRecord.dateJoined}
              max="4444-12-31"
              onChange={(e) =>
                setUpdatedRecord({ ...updatedRecord, dateJoined: e.target.value })
              }
            />
            <input
              type="email"
              className="form-control mt-2"
              value={updatedRecord.email}
              onChange={(e) =>
                setUpdatedRecord({ ...updatedRecord, email: e.target.value })
              }
            />
            <div className="d-flex justify-content-between">
              <button className="btn btn-success mt-2" onClick={handleSave}>
                Save
              </button>
              <button className="btn btn-secondary mt-2" onClick={onCancel}>
                Cancel
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  };
  
  export default EditUserForm;
  