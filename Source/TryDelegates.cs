namespace ProtoStar
{
    public delegate bool TryFunc<TResult>(out TResult result);
    public delegate bool TryFunc<in T1, TResult>(T1 t1, out TResult result);
    public delegate bool TryFunc<in T1, in T2, TResult>(T1 t1, T2 t2, out TResult result);
    public delegate bool TryFunc<in T1, in T2, in T3, TResult>(T1 t1, T2 t2, T3 t3, out TResult result);
    public delegate bool TryFunc<in T1, in T2, in T3, in T4, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, out TResult result);
}