import axios from "axios";
import { SyntheticEvent, useState } from "react"

export const Forgot = () => {
    const [email, setEmail] = useState('');
    const [notify, setNotify] = useState({
        show: false,
        error: false,
        mensagem: ''
    });

    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        try {
            await axios.post('email', {email});

            setNotify({
                show: true,
                error: false,
                mensagem: "Por favor, verifique seu email."
            });
        } catch (error) {
            
            setNotify({
                show: true,
                error: true,
                mensagem: "Ocorreu um erro"
            });
        }
    };

    let info;

    if(notify.show){
        info =  <div className={notify.error ? 'alert alert-danger': 'alert alert-success'} role="alert">
                    {notify.mensagem}
                </div>
    }

    return (
        <main className="form-signin w-100 m-auto">
            {info}
            <form onSubmit={(e) => submit(e)}>
                    <h1 
                    className="h3 mb-3 fw-normal"
                >
                    Digite seu email
                </h1>
        
                <div className="form-floating">
                    <input 
                        type="email" 
                        className="form-control" 
                        id="floatingInput3" 
                        placeholder="nome@examplo.com" 
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <label htmlFor="floatingInput3">
                        Email
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