var obj = SempleSingelton.Instace;
obj.Counter++;//то есть таким образом у нас всегда будет один и тот же обьект Counter 

//SempleSingelton singelton = new SempleSingelton();//и таким образом новые обьекты не будут создаваться 

class SempleSingelton
{
    private static SempleSingelton _instace;
    /// <summary>
    /// мы перекрываем доступ к конструктору для того ччтобы пользоватли не могу создавать каждый раз новые обьекты
    /// а пользовались всегда одним 
    /// </summary>
    SempleSingelton() { }//делаем приватный конструктор чтобы никто не смог напрямую создать этот обьект 

    public int Counter;
    public static SempleSingelton Instace//обьект можно будет создать только через это св-ва
    {
        get
        {
            if (_instace == null)
            {
                _instace = new SempleSingelton();
            }
            return _instace;
        }
    }
}


