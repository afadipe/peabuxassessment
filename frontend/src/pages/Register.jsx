import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaUser } from "react-icons/fa";
import { toast } from "react-toastify";
import { useSelector, useDispatch } from "react-redux";
import { register, reset } from "../features/auth/authSlice";
import Spinner from "../components/Spinner";

// interface RegisterFormState {
//   FirstName: string,
//   Surname: string,
//   NationIdNumber: string,
//   Password: string,
//   ConfirmPassword: string,
// }

function Register() {
  const [formData, setFormData] = useState({
    FirstName: "",
    Surname: "",
    NationIdNumber: "",
    Password: "",
    ConfirmPassword: "",
    Role: "",
    TeacherNumber: "",
    Title: 0,
    Salary: 0,
    StudentNumber: "",
    showTeacherOption: false,
    showStudentOption: false,
  });

  const {
    FirstName,
    Surname,
    NationIdNumber,
    Password,
    ConfirmPassword,
    Role,
    TeacherNumber,
    Title,
    Salary,
    StudentNumber,
    showTeacherOption,
    showStudentOption,
  } = formData;

  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { user, isLoading, isError, isSuccess, message } = useSelector(
    (state) => state.auth
  );

  useEffect(() => {
    if (isError) {
      toast.error(message);
    }

    if (isSuccess) {
      navigate("/");
    }
    dispatch(reset());
  }, [isError, isSuccess, user, message, navigate, dispatch]);

  const onChange = (e) => {
    setFormData((prevState) => ({
      ...prevState,
      [e.target.name]: e.target.value,
    }));
  };

  const onRoleChange = (e) => {
    //e.preventDefault();
    const newform = { ...formData };
    if (e.target.value === "Teacher") {
      newform.showTeacherOption = true;
      newform.showStudentOption = false;
    } else if (e.target.value === "Student") {
      newform.showTeacherOption = false;
      newform.showStudentOption = true;
    } else {
      newform.showTeacherOption = false;
      newform.showStudentOption = false;
    }
    newform.Role = e.target.value;
    setFormData(newform);
    console.log(e.target.value);
  };

  const onSubmit = (e) => {
    e.preventDefault();
    if (Password !== ConfirmPassword) {
      toast.error("Password and ConfirmPassword do not match");
    } else {
      const userData = {
        FirstName,
        Surname,
        NationIdNumber,
        Password,
        ConfirmPassword,
        Role,
        TeacherNumber,
        Title,
        Salary,
        StudentNumber,
      };
      dispatch(register(userData));
    }
  };

  if (isLoading) {
    return <Spinner />;
  }

  return (
    <>
      <section className="heading">
        <h1>
          <FaUser />
          Register
        </h1>
        <p>Please creat an account</p>
      </section>

      <section className="form">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="FirstName"
              name="FirstName"
              required
              value={FirstName}
              onChange={onChange}
              placeholder="Enter your FirstName"
            />
          </div>

          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="Surname"
              name="Surname"
              required
              value={Surname}
              onChange={onChange}
              placeholder="Enter your Surname"
            />
          </div>

          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="NationIdNumber"
              name="NationIdNumber"
              required
              value={NationIdNumber}
              onChange={onChange}
              placeholder="Enter your NationIdNumber"
            />
          </div>

          <div className="form-group">
            <input
              type="password"
              className="form-control"
              id="Password"
              name="Password"
              required
              value={Password}
              onChange={onChange}
              placeholder="Enter your Password"
            />
          </div>

          <div className="form-group">
            <input
              type="password"
              className="form-control"
              id="ConfirmPassword"
              name="ConfirmPassword"
              required
              value={ConfirmPassword}
              onChange={onChange}
              placeholder="Enter your ConfirmPassword"
            />
          </div>

          <div className="form-group">
            <select
              required
              name="Role"
              value={Role}
              id="Role"
              onChange={onRoleChange}
            >
              <option name="Role">Select Role</option>
              <option value="Teacher" name="Role">
                Teacher
              </option>
              <option value="Student" name="Role">
                Student
              </option>
            </select>
          </div>

          {showTeacherOption ? (
            <>
              <div className="form-group">
                <input
                  type="text"
                  className="form-control"
                  id="TeacherNumber"
                  name="TeacherNumber"
                  required
                  value={TeacherNumber}
                  onChange={onChange}
                  placeholder="Enter your TeacherNumber"
                />
              </div>

              <div className="form-group">
                <select
                  required
                  name="Title"
                  onChange={onChange}
                  value={Title}
                  id="Title"
                >
                  <option name="Title">Select Title</option>
                  <option value="1" name="Title">
                    Mr
                  </option>
                  <option value="2" name="Title">
                    Mrs
                  </option>
                  <option value="3" name="Title">
                    Miss
                  </option>
                  <option value="4" name="Title">
                    Prof
                  </option>
                  <option value="5" name="Title">
                    Dr
                  </option>
                </select>
              </div>

              <div className="form-group">
                <input
                  type="number"
                  className="form-control"
                  id="Salary"
                  name="Salary"
                  required
                  value={TeacherNumber}
                  onChange={onChange}
                  placeholder="Enter your Salary"
                />
              </div>
            </>
          ) : null}

          {showStudentOption ? (
            <div className="form-group">
              <input
                type="text"
                className="form-control"
                id="StudentNumber"
                name="StudentNumber"
                required
                value={StudentNumber}
                onChange={onChange}
                placeholder="Enter your StudentNumber"
              />
            </div>
          ) : null}

          <div className="form-group">
            <button className="btn btn-block"> Submit</button>
          </div>
        </form>
      </section>
    </>
  );
}

export default Register;
