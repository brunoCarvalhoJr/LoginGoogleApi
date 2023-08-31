import axios from "axios";
import { SyntheticEvent, useState } from "react";
import { Navigate, useParams } from "react-router-dom";

const Reset = () => {
    
    const [ password, setPassword ] = useState('');
    const [ confirmPassword, setConfirmPassword ] = useState('');
    const { token } = useParams();
    const [redirect, setRedirect] = useState(false);

    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        await axios.post('reset', {
            token,
            password,
            password_confirm: confirmPassword
        });

        setRedirect(true);
    };

    if(redirect){
        return <Navigate to={'/login'} />
    }

    return (
        <main className="form-signin w-100 m-auto">
            <form onSubmit={(e) => submit(e)}>
                    <h1 
                    className="h3 mb-3 fw-normal"
                >
                    Resetar sua senha
                </h1>

                <div className="form-floating">
                    <input 
                        type="password" 
                        className="form-control" 
                        id="floatingPassword1" 
                        placeholder="Senha" 
                        autoComplete="confirm-senha1"
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <label htmlFor="floatingPassword1">
                        Senha
                    </label>
                </div>
                        
                <div className="form-floating">
                <input 
                    type="password" 
                    className="form-control" 
                    id="floatingPassword2" 
                    placeholder="Confirme sua senha" 
                    autoComplete="confirm-senha"
                    onChange={(e) => setConfirmPassword(e.target.value)}
                />
                    <label htmlFor="floatingPassword2">
                        Confirmar Senha
                    </label>
                </div>
                
                <button 
                    className="btn btn-primary w-100 py-2 mt-3" 
                    type={'submit'}
                >
                    Enviar
                </button>
            </form>
        </main>
      )
}

export default Reset;