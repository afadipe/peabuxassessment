import { Link } from "react-router-dom";
import { FaUser } from "react-icons/fa";

function Home() {
  return (
    <>
      <section className="heading">
        <h1>Hello , welcome</h1>
        <h1>
          Both Teacher's and Student's should use the link below to create an account
        </h1>
      </section>
      <Link to="/register" className="btn btn-reverse btn-block">
        <FaUser /> Register
      </Link>
    </>
  );
}

export default Home;
