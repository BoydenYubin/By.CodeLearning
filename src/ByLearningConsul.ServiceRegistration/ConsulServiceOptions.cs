namespace ByLearningConsul.ServiceRegistration
{
    public class ConsulServiceOptions
    {
        public string ConsulAddress { get; set; }
        public string ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string HealthCheck { get; set; }
    }
}