﻿namespace Icp.TiendaApi.Servicios.Almacenador
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor htppContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor htppContextAccessor)
        {
            this.env = env;
            this.htppContextAccessor = htppContextAccessor;
        }

        public Task BorrarAchivo(string ruta, string contenedor)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorio = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);

                if (File.Exists(directorio))
                {
                    File.Delete(directorio);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarAchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);

        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{htppContextAccessor.HttpContext.Request.Scheme}://{htppContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
        }
    }
}
