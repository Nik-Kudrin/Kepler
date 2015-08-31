namespace Kepler.Tests.FixtureBuilder
{
    public interface IFixtureBuilder<T>
    {
        T BuildValid();
    }
}